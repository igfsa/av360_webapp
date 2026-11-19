/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { GrupoRealTimeService } from './GrupoRealTime.service';

describe('Service: GrupoRealTime', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GrupoRealTimeService]
    });
  });

  it('should ...', inject([GrupoRealTimeService], (service: GrupoRealTimeService) => {
    expect(service).toBeTruthy();
  }));
});
