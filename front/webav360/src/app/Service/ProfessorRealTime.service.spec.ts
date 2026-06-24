/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ProfessorRealTimeService } from './ProfessorRealTime.service';

describe('Service: ProfessorRealTime', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProfessorRealTimeService]
    });
  });

  it('should ...', inject([ProfessorRealTimeService], (service: ProfessorRealTimeService) => {
    expect(service).toBeTruthy();
  }));
});
