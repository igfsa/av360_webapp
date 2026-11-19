/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AvaliacaoService } from './Avaliacao.service';

describe('Service: Avaliacao', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AvaliacaoService]
    });
  });

  it('should ...', inject([AvaliacaoService], (service: AvaliacaoService) => {
    expect(service).toBeTruthy();
  }));
});
