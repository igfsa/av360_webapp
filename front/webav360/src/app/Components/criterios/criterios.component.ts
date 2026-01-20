import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CriterioService } from '../../Service/Criterio.service';
import { Criterio } from '../../Models/Criterio';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-criterios',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
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
    private cdr: ChangeDetectorRef
  ){}

  ngOnInit() {
    void this.getCriterios();
  }

  public getCriterios (): void{
    this.criterioService.getCriterios().subscribe({
      next: (a: Criterio[]) =>
      {
        this.criterios = a;
        this.criteriosFiltrados = this.criterios;

        this.cdr.detectChanges();
      },
      error: (e) => console.log(e)
    })
  }

}
