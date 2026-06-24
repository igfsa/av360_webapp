import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ChangeDetectorRef, Component, DestroyRef, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import Swal from 'sweetalert2';
import { AuthService } from '../../auth/auth.service';
import { Professor } from '../../Models/Professor';
import { ProfessorService } from '../../Service/Professor.service';
import { ProfessorRealTime } from '../../Service/ProfessorRealTime.service';
import { FormsModule } from '@angular/forms';
import { ProfessorCriarModalComponent } from './modals/professor_criar.component';

@Component({
  selector: 'app-professores',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
   ],
  templateUrl: './professores.component.html',
  styleUrls: ['./professores.component.scss']
})
export class ProfessoresComponent implements OnInit {

  public professores: Professor[]  = [];
  public professoresFiltrados : Professor[] = [];
  private _filtroLista: string = '';

  public get filtroLista() {
    return this._filtroLista
  }

  public set filtroLista(value : string) {
    this._filtroLista = value;
    this.professoresFiltrados = this.filtroLista ? this.filtrarProfessores(this.filtroLista) : this.professores;
  }

  public filtrarProfessores(filtrarPor: string): any {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.professores.filter(
      (professor: { nome: string; }) => professor.nome.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  constructor(
    private professorService: ProfessorService,
    private cdr: ChangeDetectorRef,
    private professorRealTime: ProfessorRealTime,
    private authService: AuthService,
    @Inject(NgbModal) private modalService: NgbModal,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef
  ){}

  ngOnInit() {
    if (this.authService.isLogged())
    {
      this.getProfessores();

      if (isPlatformBrowser(this.platformId)) {
        this.professorRealTime.connect();

        this.professorRealTime.professorAtualizado$
          .pipe(takeUntilDestroyed(this.destroyRef))
          .subscribe(id => {
            if (id) {
              this.getProfessores();
            }
          });
    }};
  }

  ngOnDestroy(): void {
    this.professorRealTime.disconnect();
  }

  public getProfessores (): void{
    this.professorService.getProfessores().subscribe((professores) => {
        this.professores = professores;
        this.professoresFiltrados = this.professores;

        this.cdr.detectChanges();
      })
  }

  public adicionarProfessor (): void{
    const ref = this.modalService.open(ProfessorCriarModalComponent, {
      size: 'lg',
      backdrop: 'static',
      centered: true,
      fullscreen: true,
      scrollable: true
    });

    ref.result.then((novoProfessor: Professor) => {
      if (!novoProfessor) return;

    this.professorService.postProfessor(novoProfessor)
      .subscribe({
        next: professor => {
          Swal.mixin({
            toast: true,
            position: "top-end",
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            didOpen: (toast) => {
              toast.onmouseenter = Swal.stopTimer;
              toast.onmouseleave = Swal.resumeTimer;
            }
          }).fire({
            icon: 'success',
            title: 'Sucesso',
            text: `Professor ${professor.nome} criado com sucesso!`
          });
        },
        error: (err) => {
          Swal.fire({
            icon: 'error',
            title: 'Erro',
            text: err.error?.message ?? `Erro ao criar professor ${novoProfessor.nome}`
          });
        }
      });
    }).catch(() => {});
  }

}
