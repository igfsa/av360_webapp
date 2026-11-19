import { ChangeDetectorRef, Component, DestroyRef, Inject, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { TableModule } from "primeng/table";

import { LoadingComponent } from '@shared/components/loading/loading.component';
import { AuthService } from '@core/auth/auth.service';
import { CriterioCriarModalComponent } from './modals/criterio_criar.component';
import { CriterioRealTime } from '@core/realtime/CriterioRealTime.service';
import { CriterioEditarModalComponent } from '@features/professor/shared/modals/criterio_editar.component';
import { CriterioService } from '@shared/services/Criterio.service';
import { convertCriterio, Criterio } from '@shared/models/Criterio';
import { ModalService } from '@shared/components/modal/modal.service';
import { AlertService } from '@shared/components/alert/alert.service';

@Component({
  selector: 'app-criterios',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    LoadingComponent,
    TableModule
],
  templateUrl: './criterios.component.html',
  styleUrls: ['./criterios.component.scss', '../../../app.scss'],
})
export class CriteriosComponent implements OnInit, OnDestroy {

  protected readonly asCriterio = convertCriterio;

  public criterios: Criterio[]  = [];
  public loading: boolean = true;

  Id: string = '';
  Nome: string = '';

  constructor(
    private criterioService: CriterioService,
    private cdr: ChangeDetectorRef,
    private criterioRealTime: CriterioRealTime,
    private authService: AuthService,
    private modal: ModalService,
    private alert: AlertService,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef
  ){}

  ngOnInit() {
    if (this.authService.usuario())
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

  ngOnDestroy(): void {
    this.criterioRealTime.disconnect();
  }

  public getCriterios (): void{
    this.criterioService.getCriteriosProfessor().subscribe((criterios) => {
        this.criterios = criterios;
        this.loading = false;
        this.cdr.detectChanges();
      })
  }

  public adicionarCriterio (): void{
    this.criterioService.getCriteriosSugestoes().subscribe((res) => {
      this.modal.open<string[], Criterio>(
        CriterioCriarModalComponent,
        res ,
        { header: `Adicionar Critério` }
      ).subscribe((criterio) => {
        if (!criterio) return;

      this.criterioService.postCriterio(criterio)
        .subscribe({
          next: criterio => {
            this.alert.toastSuccess(`Critério ${criterio.nome} criado com sucesso!`);
          },
          error: (err) => {
            this.alert.error(err.error?.message ?? `Erro ao criar critério ${criterio.nome}`);
          }
        });
      });
    })
  }

  public editarCriterio (criterio: Criterio): void{
    this.modal.open<Criterio, Criterio>(
      CriterioEditarModalComponent,
      criterio ,
      { header: `Editar Critério` }
    ).subscribe((criterioEditado) => {
      if (!criterioEditado) return;

      this.criterioService.putCriterio(criterioEditado).subscribe({
        next: (c) => {
          criterio = c;
          this.alert.toastSuccess(`Critério ${c.nome} editado com sucesso!`);
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao editar critério ${criterioEditado.nome}`);
        }
      });
    });
  }
}
