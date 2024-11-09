import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TermsService {

  getTerms(): Observable<string> {
    return of(this.mockTermsData());
  }

  private mockTermsData(): string {
    return `
      <h6>1. Introduction</h6>
      <p>Ces termes et conditions régissent votre utilisation de notre site web; en utilisant notre site web, vous acceptez ces termes et conditions dans leur intégralité.
      Si vous êtes en désaccord avec ces termes et conditions ou une partie de ces termes et conditions, vous ne devez pas utiliser notre site web.</p>
      <h6>2. Licence d'utilisation du site</h6>
      <p>À moins que cela ne soit spécifiquement indiqué, nous ou nos concédants possédons les droits de propriété intellectuelle sur le site et le matériel sur le site.
      Sous réserve de la licence ci-dessous, tous ces droits de propriété intellectuelle sont réservés.</p>
      <h6>3. Utilisation acceptable</h6>
      <p>Vous ne devez pas utiliser notre site web de quelque manière que ce soit qui cause, ou pourrait causer, des dommages au site web ou une altération de la disponibilité ou de l'accessibilité du site;
      ou de toute manière qui soit illégale, illégale, frauduleuse ou nuisible, ou en relation avec tout but ou activité illégale, illégale, frauduleuse ou nuisible.</p>
      <h6>4. Accès restreint</h6>
      <p>L'accès à certaines zones de notre site web est restreint. Nous nous réservons le droit de restreindre l'accès à d'autres zones de notre site web, voire à l'ensemble de notre site web, à notre discrétion.
      Si nous vous fournissons un identifiant utilisateur et un mot de passe pour vous permettre d'accéder à des zones restreintes de notre site web ou à d'autres contenus ou services,
      vous devez vous assurer que cet identifiant utilisateur et ce mot de passe restent confidentiels.</p>
      <h6>5. Contenu utilisateur</h6>
      <p>Dans ces termes et conditions, “votre contenu utilisateur” signifie du matériel (y compris, sans limitation, texte, images, matériel audio, matériel vidéo et matériel audio-visuel)
      que vous soumettez à notre site web, pour quelque fin que ce soit.</p>
      <h6>6. Absence de garanties</h6>
      <p>Ce site web est fourni “tel quel” sans aucune représentation ou garantie, expresse ou implicite. Nous ne faisons aucune représentation ou garantie en ce qui concerne ce site web
      ou les informations et matériaux fournis sur ce site web.</p>
      <h6>7. Limitations de responsabilité</h6>
      <p>Nous ne serons pas responsables envers vous (que ce soit en vertu de la loi du contrat, de la loi des délits ou autrement) en relation avec le contenu de, ou l'utilisation de, ou autrement en relation avec,
      ce site web: dans la mesure où le site web est fourni gratuitement, pour toute perte directe; pour toute perte indirecte, spéciale ou consécutive; ou pour toute perte commerciale,
      perte de revenus, de revenus, de bénéfices ou d'économies anticipées, perte de contrats ou de relations commerciales, perte de réputation ou de bonne volonté, ou perte ou corruption d'informations ou de données.</p>
      <h6>8. Indemnité</h6>
      <p>Vous nous indemnisez par la présente et vous vous engagez à nous tenir indemnes contre toute perte, tout dommage, coût, responsabilité et dépense (y compris, sans limitation, les frais juridiques et tout montant
      payé par nous à un tiers en règlement d'une réclamation ou d'un litige sur les conseils de nos conseillers juridiques) encourus ou subis par nous suite à une violation par vous de toute disposition de ces termes et conditions.</p>
      <h6>9. Violations de ces termes et conditions</h6>
      <p>Sans préjudice de nos autres droits en vertu de ces termes et conditions, si vous violez ces termes et conditions de quelque manière que ce soit, nous pouvons prendre les mesures que nous jugeons appropriées pour faire face à la violation,
      y compris suspendre votre accès au site web, vous interdire d'accéder au site web, bloquer les ordinateurs utilisant votre adresse IP d'accéder au site web,
      contacter votre fournisseur de services internet pour demander qu'il bloque votre accès au site web et/ou intenter une procédure judiciaire contre vous.</p>
    `;
  }
}
