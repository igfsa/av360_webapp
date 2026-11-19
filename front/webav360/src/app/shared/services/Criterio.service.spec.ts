/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { CriterioService } from './Criterio.service';

describe('Service: Criterio', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CriterioService]
    });
  });

  it('should ...', inject([CriterioService], (service: CriterioService) => {
    expect(service).toBeTruthy();
  }));
});
