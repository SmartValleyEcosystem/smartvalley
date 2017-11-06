import { NgModule }              from '@angular/core';
import { RouterModule, Routes }  from '@angular/router';
import {TestComponent} from "./components/test/test.component";

const appRoutes: Routes = [
  { path: '',  pathMatch: 'full', component: TestComponent },
];

@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {}
