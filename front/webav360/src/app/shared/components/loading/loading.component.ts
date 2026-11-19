import { Component } from '@angular/core';
import { SkeletonModule } from 'primeng/skeleton';

@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [
    SkeletonModule
  ],
  template: `
  <div class="d-flex justify-content-between w-100 my-4">
    <p-skeleton size="20vh" borderRadius="16px" />
    <p-skeleton size="20vh" borderRadius="16px" />
    <p-skeleton size="20vh" borderRadius="16px" />
  </div>

  <div class="m-100 my-4">
    <p-skeleton width="60%" height="2rem" borderRadius="16px" class="mb-3" />
    <p-skeleton width="25%" height="2rem" borderRadius="16px" class="mb-3" />
    <p-skeleton width="100%" height="2rem" borderRadius="16px" class="mb-3" />
  </div>

  <div class="w-100 my-4">

    @for (_ of [1,2,3,4,5]; track $index) {

      <div class="d-flex justify-content-between mb-3" >

        <p-skeleton width="30%" height="2rem" borderRadius="16px" />

        <p-skeleton width="30%" height="2rem" borderRadius="16px" />

        <p-skeleton width="30%" height="2rem" borderRadius="16px" />

      </div>

    }

  </div>
  `,
  styles: ''
})
export class LoadingComponent {

}
