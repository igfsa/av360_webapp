import { ChangeDetectorRef, Component, DestroyRef, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../../auth/auth.service';
import { SessaoService } from '../../../Service/Sessao.service';
import { SessaoRealTime } from '../../../Service/SessaoRealTime.service';
import { Sessao } from '../../../Models/Sessao';
import { isPlatformBrowser } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { API_URL, FRONT_URL } from '../../../app.config';

@Component({
  selector: 'sessao-qrcode',
  standalone: true,
  imports: [],
  template: `
    <div class="container">
      @if (!carregando) {
        @if (sessaoAtiva && sessaoAtiva.ativo) {
          <h1>Turma {{sessaoAtiva.turma.cod}}</h1>
          <figure class="figure w-100 p-2">
            <figcaption class="figure-caption">{{url}}</figcaption>
            <img [src]="qrCode"
              alt="QR Code da Avaliação"
              class="figure-img img-fluid rounded w-50"
              style="margin: 0 25%;">
          </figure>
        } @else {
          <h2>Sessão não encontrada ou inválida</h2>
        }
      }
    </div>
  `
})
export class SessaoQrCodeComponent implements OnInit {

  public qrCode: string = '';
  public url: string = '';
  public sessaoAtiva?: Sessao;
  public carregando: boolean = true;

  constructor(
    private sessaoService: SessaoService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef,
    private sessaoRealTime: SessaoRealTime,
    public authService: AuthService,
    @Inject(PLATFORM_ID) private platformId: Object,
    @Inject(DestroyRef) private destroyRef: DestroyRef,
    @Inject(API_URL) public readonly baseURL: string,
    @Inject(FRONT_URL) public readonly frontURL: string
  ) {}

  ngOnInit(): void {
    if (this.authService.isLogged()){
      const sessaoId = Number(this.route.snapshot.paramMap.get('id'));

      this.loadData(sessaoId);

      if (isPlatformBrowser(this.platformId) && this.sessaoAtiva) {
        this.sessaoRealTime.connect()?.then(() => {
          this.sessaoRealTime.acessarSessao(sessaoId);
        });

        this.sessaoRealTime.sessaoFinalizada$
          .pipe(takeUntilDestroyed(this.destroyRef))
          .subscribe(id => {
            if (id === sessaoId) {
              this.loadData(sessaoId);
            }
        });
    }}

  }

  public loadData(sessaoId: number) {
      this.sessaoService.getSessaoId(sessaoId).subscribe((sessaoAtiva) => {
      this.sessaoAtiva = sessaoAtiva;

      if (sessaoAtiva.ativo){
        this.qrCode = `${this.baseURL}/api/Sessao/GetQrCode/${sessaoAtiva.id}`;
        this.url = `${this.frontURL}/avaliacao/publica/${sessaoAtiva.tokenPublico}`;

        this.sessaoRealTime.connect()?.then(() => {
          this.sessaoRealTime.acessarSessao(sessaoAtiva.id);
      });

        this.sessaoRealTime.sessaoFinalizada$
          .subscribe(id => {
            if (id === sessaoAtiva.id) {

            }
        });
      }

      this.carregando = false;
      this.cdr.detectChanges();
    });
  }
}
