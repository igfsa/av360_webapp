/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TurmaRealTime.service.tsService } from './TurmaRealTime.service.ts.service';

describe('Service: TurmaRealTime.service.ts', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TurmaRealTime.service.tsService]
    });
  });

  it('should ...', inject([TurmaRealTime.service.tsService], (service: TurmaRealTime.service.tsService) => {
    expect(service).toBeTruthy();
  }));
});
