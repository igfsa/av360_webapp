import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ChangeDetectorRef, Component, DestroyRef, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { TableModule } from "primeng/table";


import { AuthService } from '../../auth/auth.service';
import { Professor } from '../../Models/Professor';
import { ProfessorService } from '../../Service/Professor.service';
import { ProfessorRealTime } from '../../Service/ProfessorRealTime.service';
import { FormsModule } from '@angular/forms';
import { ProfessorCriarModalComponent } from './modals/professor_criar.component';
import { LoadingComponent } from "../shared/loading/loading.component";
import { ModalService } from '../shared/modal/modal.service';
import { AlertService } from '../shared/alert/alert.service';

@Component({
  selector: 'app-professores',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    LoadingComponent,
    TableModule
],
  templateUrl: './professores.component.html',
  styleUrls: ['./professores.component.scss']
})
export class ProfessoresComponent implements OnInit {

  public professores: Professor[]  = [];
  public loading: boolean = true;

  constructor(
    private professorService: ProfessorService,
    private cdr: ChangeDetectorRef,
    private professorRealTime: ProfessorRealTime,
    private authService: AuthService,
    private modal: ModalService,
    private alert: AlertService,
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
        this.loading = false;
        this.cdr.detectChanges();
      })
  }

  public adicionarProfessor (): void{
    this.modal.open<null, Professor>(
      ProfessorCriarModalComponent,
      null,
      { header: `Adicionar Professor`}
    ).subscribe((novoProfessor) => {
      if (!novoProfessor) return;

    this.professorService.postProfessor(novoProfessor)
      .subscribe({
        next: professor => {
          this.alert.toastSuccess(`Professor ${professor.nome} criado com sucesso!`);
        },
        error: (err) => {
          this.alert.error(err.error?.message ?? `Erro ao criar professor ${novoProfessor.nome}`);
        }
      });
    });
  }
}
