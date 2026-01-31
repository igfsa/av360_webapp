/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { GrupoService } from './Grupo.service';

describe('Service: Grupo', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GrupoService]
    });
  });

  it('should ...', inject([GrupoService], (service: GrupoService) => {
    expect(service).toBeTruthy();
  }));
});
