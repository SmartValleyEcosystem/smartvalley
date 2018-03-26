import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AuthenticationService} from '../../../services/authentication/authentication-service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  public form: FormGroup;

  constructor(private formBuilder: FormBuilder,
              private authenticationService: AuthenticationService) {
    this.form = this.formBuilder.group({
      email: ['', [
        Validators.required,
        Validators.email]
      ],
    });
  }

  async submitAsync() {
    await this.authenticationService.registerAsync(this.form.value.email);
  }
}
