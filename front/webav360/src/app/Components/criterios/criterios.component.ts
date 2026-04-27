import { ChangeDetectorRef, Component, DestroyRef, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CriterioService } from '../../Service/Criterio.service';
import { Criterio } from '../../Models/Criterio';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CriterioCriarModalComponent } from './Modals/criterio_criar.component';
import { CriterioRealTime } from '../../Service/CriterioRealTime.service';
import { CriterioEditarModalComponent } from './Modals/criterio_editar.component';
import Swal from 'sweetalert2';
import { AuthService } from '../../auth/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-criterios',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
   ],
  templateUrl: './criterios.component.html',
  styleUrls: ['./criterios.component.scss', '../../app.scss'],
})
export class CriteriosComponent implements OnInit {

  public criterios: Criterio[]  = [];
  public criteriosFiltrados : Criterio[] = [];
  private _filtroLista: string = '';

  public get filtroLista() {
    return this._filtroLista
  }

  public set filtroLista(value : string) {
    this._filtroLista = value;
    this.criteriosFiltrados = this.filtroLista ? this.filtrarCriterios(this.filtroLista) : this.criterios;
  }

  public filtrarCriterios(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.criterios.filter(
      (criterio: { nome: string; }) => criterio.nome.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  Id: string = '';
  Nome: string = '';

  constructor(
    private criterioService: CriterioService,
    private cdr: ChangeDetectorRef,
    private criterioRealTime: CriterioRealTime,
    private authService: AuthService,
    @Inject(NgbModal) private modalService: NgbModal,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef
  ){}

  ngOnInit() {
    if (this.authService.isLogged())
    {
      this.getCriterios();

      if (isPlatformBrowser(this.platformId)) {
        this.criterioRealTime.connect();

        this.criterioRealTime.criterioAtualizado$
          .pipe(takeUntilDestroyed(this.destroyRef))
          .subscribe(id => {
            if (id) {
              this.getCriterios();
            }
          });
    }};
  }

  public getCriterios (): void{
    this.criterioService.getCriterios().subscribe((criterios) => {
        this.criterios = criterios;
        this.criteriosFiltrados = this.criterios;

        this.cdr.detectChanges();
      })
  }

  public adicionarCriterio (): void{
    const ref = this.modalService.open(CriterioCriarModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true
    });

    ref.result.then((criterioEditado: Criterio) => {
      if (!criterioEditado) return;

    this.criterioService.postCriterio(criterioEditado)
      .subscribe({
        next: criterio => {
          Swal.fire({
            icon: 'success',
            title: 'Sucesso',
            text: `Critério ${criterio.nome} criado com sucesso!`
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao criar critério ${criterioEditado.nome}`
          });
        }
      });
    }).catch(() => {});
  }

  public editarCriterio (criterio: Criterio): void{
    const ref = this.modalService.open(CriterioEditarModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true
    });

    ref.componentInstance.criterio = criterio;

    ref.result.then((criterioEditado: Criterio) => {
      if (!criterioEditado) return;

      this.criterioService.putCriterio(criterioEditado).subscribe({
        next: (c) => {
          criterio = c;
          Swal.fire({
            icon: 'success',
            title: 'Sucesso',
            text: `Critério ${c.nome} editado com sucesso!`
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao editar critério ${criterioEditado.nome}`
          });
        }
      });
    }).catch(() => {});
  }
}
