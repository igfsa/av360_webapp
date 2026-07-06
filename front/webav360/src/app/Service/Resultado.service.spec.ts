/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ResultadoService } from './Resultado.service';

describe('Service: Resultado', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ResultadoService]
    });
  });

  it('should ...', inject([ResultadoService], (service: ResultadoService) => {
    expect(service).toBeTruthy();
  }));
});
