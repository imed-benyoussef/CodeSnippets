import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Fido2Service } from '../fido2.service';

@Component({
  selector: 'app-fido2-registration',
  templateUrl: './fido2-registration.component.html'
})
export class Fido2RegistrationComponent implements OnInit {
  registrationForm!: FormGroup;
  isRegistering = false;
  errorMessage = '';
  statusMessage = '';

  constructor(
    private fb: FormBuilder,
    private fido2Service: Fido2Service,
    private router: Router
  ) {}

  ngOnInit() {
    this.registrationForm = this.fb.group({
      keyName: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  async onSubmit() {
    if (this.registrationForm.invalid) return;

    this.isRegistering = true;
    this.errorMessage = '';
    this.statusMessage = 'Démarrage de l\'enregistrement...';

    try {
      const options = await this.fido2Service.startRegistration(
        this.registrationForm.value.keyName
      ).toPromise();

      const credential = await navigator.credentials.create({
        publicKey: this.preformatPublicKeyCredentialCreationOptions(options)
      });

      if (credential) {
        await this.fido2Service.completeRegistration(
          this.preformatCredentialResponse(credential)
        ).toPromise();

        this.statusMessage = 'Enregistrement réussi!';
        setTimeout(() => this.router.navigate(['/profile']), 2000);
      }
    } catch (error:any) {
      this.errorMessage = 'Erreur lors de l\'enregistrement: ' + error.message;
    } finally {
      this.isRegistering = false;
    }
  }

  onCancel() {
    this.router.navigate(['/profile']);
  }

  private preformatPublicKeyCredentialCreationOptions(options: any): any {
    // Conversion des données en ArrayBuffer
    options.challenge = Uint8Array.from(
      atob(options.challenge), c => c.charCodeAt(0)
    );
    options.user.id = Uint8Array.from(
      atob(options.user.id), c => c.charCodeAt(0)
    );
    return options;
  }

  private preformatCredentialResponse(credential: any): any {
    // Conversion des données pour l'API
    return {
      id: credential.id,
      rawId: btoa(String.fromCharCode(...new Uint8Array(credential.rawId))),
      response: {
        attestationObject: btoa(
          String.fromCharCode(...new Uint8Array(credential.response.attestationObject))
        ),
        clientDataJSON: btoa(
          String.fromCharCode(...new Uint8Array(credential.response.clientDataJSON))
        )
      },
      type: credential.type
    };
  }
}
