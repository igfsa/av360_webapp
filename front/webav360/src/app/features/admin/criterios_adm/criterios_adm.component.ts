import { ChangeDetectorRef, Component, DestroyRef, Inject, OnDestroy, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { TableModule } from "primeng/table";

import { LoadingComponent } from '@shared/components/loading/loading.component';
import { AuthService } from '@core/auth/auth.service';
import { CriterioAdmCriarModalComponent } from './modals/criterio_adm_criar.component';
import { CriterioRealTime } from '@core/realtime/CriterioRealTime.service';
import { CriterioAdmEditarModalComponent } from './modals/criterio_adm_editar.component';
import { CriterioService } from '@shared/services/Criterio.service';
import { Criterio } from '@shared/models/Criterio';
import { convertCriterioAdm, CriterioAdm } from './models/CriterioAdm';
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
  templateUrl: './criterios_adm.component.html',
  styleUrls: ['./criterios_adm.component.scss', '../../../app.scss'],
})
export class CriteriosAdmComponent implements OnInit, OnDestroy {

  protected readonly asCriterioAdm = convertCriterioAdm;

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
    this.criterioService.getCriteriosAdm().subscribe((criterios) => {
        this.criterios = criterios;
        this.loading = false;
        this.cdr.detectChanges();
      })
  }

  public adicionarCriterio (): void{
    this.modal.open<null, Criterio>(
      CriterioAdmCriarModalComponent,
      null,
      { header: `Adicionar Critério` }
    ).subscribe((criterio) => {
      if (!criterio) return;

    this.criterioService.postCriterioAdm(criterio)
      .subscribe({
        next: criterio => {
          this.alert.toastSuccess(`Critério ${criterio.nome} criado com sucesso!`);
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao criar critério ${criterio.nome}`);
        }
      });
    });
  }

  public editarCriterio (criterio: Criterio): void{
    this.modal.open<Criterio, Criterio>(
      CriterioAdmEditarModalComponent,
      criterio ,
      { header: `Editar Critério` }
    ).subscribe((criterioEditado) => {
      if (!criterioEditado) return;

      this.criterioService.putCriterioAdm(criterioEditado).subscribe({
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
