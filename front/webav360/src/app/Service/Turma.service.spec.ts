/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TurmaService } from './Turma.service';

describe('Service: Turma', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TurmaService]
    });
  });

  it('should ...', inject([TurmaService], (service: TurmaService) => {
    expect(service).toBeTruthy();
  }));
});
