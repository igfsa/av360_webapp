/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { _GlobalVariablesService } from './_GlobalVariables.service';

describe('Service: _GlobalVariables', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [_GlobalVariablesService]
    });
  });

  it('should ...', inject([_GlobalVariablesService], (service: _GlobalVariablesService) => {
    expect(service).toBeTruthy();
  }));
});
