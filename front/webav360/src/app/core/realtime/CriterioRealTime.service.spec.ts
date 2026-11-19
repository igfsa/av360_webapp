/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { CriterioRealTimeService } from './CriterioRealTime.service';

describe('Service: CriterioRealTime', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CriterioRealTimeService]
    });
  });

  it('should ...', inject([CriterioRealTimeService], (service: CriterioRealTimeService) => {
    expect(service).toBeTruthy();
  }));
});
