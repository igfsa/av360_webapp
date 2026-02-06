/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { Avaliacao_publicaComponent } from './avaliacao_publica.component';

describe('Avaliacao_publicaComponent', () => {
  let component: Avaliacao_publicaComponent;
  let fixture: ComponentFixture<Avaliacao_publicaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Avaliacao_publicaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Avaliacao_publicaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
