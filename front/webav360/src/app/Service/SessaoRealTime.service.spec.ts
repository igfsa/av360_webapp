/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SessaoRealTimeService } from './SessaoRealTime.service';

describe('Service: SessaoRealTime', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SessaoRealTimeService]
    });
  });

  it('should ...', inject([SessaoRealTimeService], (service: SessaoRealTimeService) => {
    expect(service).toBeTruthy();
  }));
});
