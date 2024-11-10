import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ConsentService } from '../services/consent.service';

@Component({
  selector: 'app-consent',
  templateUrl: './consent.component.html'
})
export class ConsentComponent implements OnInit {
  requestedScopes: string[] = [];
  clientInfo: any;
  errorMessage?: string;
  returnUrl?: string;
  consentForm: FormGroup;

  constructor(
    private consentService: ConsentService,
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder
  ) {
    this.consentForm = this.fb.group({
      rememberChoice: [false]
    });
  }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      if (!params['client_id'] || !params['return_url']) {
        this.errorMessage = 'Paramètres de requête invalides';
        return;
      }
      this.returnUrl = params['return_url'];
      this.requestedScopes = params['scope']?.split(' ') || [];
      this.loadClientInfo(params['client_id']);
    });
  }

  async approve() {
    try {
      await this.consentService.giveConsent({
        scopes: this.requestedScopes,
        remember: this.consentForm.get('rememberChoice')?.value
      });
      window.location.href = this.returnUrl + '?consent=approved';
    } catch (error) {
      this.errorMessage = 'Erreur lors de l\'approbation du consentement';
    }
  }

  async deny() {
    try {
      await this.consentService.denyConsent();
      window.location.href = this.returnUrl + '?consent=denied';
    } catch (error) {
      this.errorMessage = 'Erreur lors du refus du consentement';
    }
  }

  private async loadClientInfo(clientId: string) {
    try {
      this.clientInfo = await this.consentService.getClientInfo(clientId);
      if (!this.clientInfo) {
        this.errorMessage = 'Application cliente non trouvée';
      }
    } catch (error) {
      this.errorMessage = 'Erreur lors du chargement des informations client';
    }
  }
}