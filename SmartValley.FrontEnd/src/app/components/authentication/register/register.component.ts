import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthenticationService} from '../../../services/authentication/authentication-service';
import {ActivatedRoute} from '@angular/router';
import {isNullOrUndefined} from 'util';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  public form: FormGroup;
  private readonly canCreatePrivateProjects: boolean = false;

  constructor(private formBuilder: FormBuilder,
              private authenticationService: AuthenticationService,
              private activatedRoute: ActivatedRoute) {
    if (!isNullOrUndefined(this.activatedRoute.snapshot.queryParams.code)) {
      localStorage.setItem('canCreatePrivateProjects', 'true');
    }

    if (!isNullOrUndefined(localStorage.getItem('canCreatePrivateProjects')) &&
      JSON.parse(localStorage.getItem('canCreatePrivateProjects'))) {
      this.canCreatePrivateProjects = true;
    }

    this.form = this.formBuilder.group({
      email: ['', [
        Validators.required,
        Validators.email]
      ],
    });
  }

  async submitAsync() {

    await this.authenticationService.authenticateAsync();

    if (this.form.invalid) {
      return;
    }
    await this.authenticationService.registerAsync(this.form.value.email, this.canCreatePrivateProjects);
  }
}
