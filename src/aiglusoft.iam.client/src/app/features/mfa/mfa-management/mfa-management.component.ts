import { Component, OnInit } from '@angular/core';
import { MfaService, MfaMethod } from '../services/mfa.service';

@Component({
  selector: 'app-mfa-management',
  templateUrl: './mfa-management.component.html'
})
export class MfaManagementComponent implements OnInit {
  isMfaEnabled = false;
  message = '';
  messageType: 'success' | 'error' = 'success';
  isLoading = false;
  enabledMethods: MfaMethod[] = [];

  constructor(private mfaService: MfaService) {}

  ngOnInit(): void {
    this.loadMfaStatus();
    this.loadEnabledMethods();
  }

  private loadMfaStatus(): void {
    this.mfaService.getMfaStatus().subscribe({
      next: (status) => {
        this.isMfaEnabled = status;
      },
      error: (error) => {
        this.showMessage('Erreur lors du chargement du statut MFA', 'error');
      }
    });
  }

  private loadEnabledMethods(): void {
    this.mfaService.getEnabledMethods().subscribe({
      next: (methods) => {
        this.enabledMethods = methods;
      },
      error: (error) => {
        this.showMessage('Erreur lors du chargement des méthodes MFA', 'error');
      }
    });
  }

  toggleMfa(): void {
    this.isLoading = true;
    this.mfaService.toggleMfa(!this.isMfaEnabled).subscribe({
      next: () => {
        this.isMfaEnabled = !this.isMfaEnabled;
        this.showMessage(`2FA ${this.isMfaEnabled ? 'activée' : 'désactivée'} avec succès`, 'success');
      },
      error: (error) => {
        this.showMessage('Erreur lors de la modification du statut 2FA', 'error');
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  configureTotpApp(): void {
    this.mfaService.setupTotp().subscribe({
      next: (response) => {
        // Gérer l'affichage du QR code et la configuration
        this.showMessage('Configuration TOTP initiée', 'success');
      },
      error: (error) => {
        this.showMessage('Erreur lors de la configuration TOTP', 'error');
      }
    });
  }

  configureSms(): void {
    // Implémenter la logique de configuration SMS
    // Vous pourriez ouvrir un dialogue pour saisir le numéro de téléphone
  }

  configureFido2(): void {
    this.isLoading = true;
    this.mfaService.setupFido2().subscribe({
      next: async (response) => {
        try {
          // Logique spécifique à l'API WebAuthn
          const credential = await navigator.credentials.create({
            publicKey: this.preformatFido2Challenge(response.challenge)
          });
          
          await this.mfaService.verifyFido2(credential).toPromise();
          this.showMessage('Clé FIDO2 configurée avec succès', 'success');
          this.loadEnabledMethods();
        } catch (error) {
          this.showMessage('Erreur lors de la configuration FIDO2', 'error');
        }
      },
      error: (error) => {
        this.showMessage('Erreur lors de l\'initialisation FIDO2', 'error');
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  private preformatFido2Challenge(challenge: string): PublicKeyCredentialCreationOptions {
    // Implémentation de la conversion du challenge pour WebAuthn
    // À adapter selon vos besoins spécifiques
    return {
      challenge: new Uint8Array(challenge.split(',').map(Number)).buffer,
      // Autres options de configuration WebAuthn
      rp: { name: 'Votre App', id: window.location.hostname },
      user: {
        id: new Uint8Array(16),
        name: 'user@example.com',
        displayName: 'Utilisateur'
      },
      pubKeyCredParams: [
        { type: 'public-key', alg: -7 }
      ],
      timeout: 60000,
      attestation: 'direct'
    };
  }

  private showMessage(text: string, type: 'success' | 'error'): void {
    this.message = text;
    this.messageType = type;
    setTimeout(() => this.message = '', 5000);
  }
}
