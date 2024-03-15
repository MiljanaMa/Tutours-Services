import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';
import { User } from 'src/app/infrastructure/auth/model/user.model';
import { NewsletterPreference } from '../model/newsletter-preference.model';
import { TouristService } from '../tourist.service';

@Component({
  selector: 'xp-newsletter-preference',
  templateUrl: './newsletter-preference.component.html',
  styleUrls: ['./newsletter-preference.component.css']
})
export class NewsletterPreferenceComponent {
  newsletterForm: FormGroup
  user: User | undefined
  existingPreference: NewsletterPreference = {
    userid: -1,
    frequency: 0,
    lastSent: new Date('1990-01-01')
  }

  public constructor(private touristService: TouristService, private authService: AuthService)
  {
    this.newsletterForm = new FormGroup({
      frequency: new FormControl('0', [Validators.required]),
    });
    
    this.authService.user$.subscribe(user => {
      this.user = user;
      touristService.getNewsletterPreference(user.id).subscribe(
        (res: NewsletterPreference) => {
          this.existingPreference = res;
          this.newsletterForm.setValue({frequency:res.frequency.toString()})
        }
      )
    });
  }

  reset()
  {
    this.newsletterForm.setValue({frequency:'0'})
  }

  getLastSentDate()
  {
    if(this.existingPreference !== null || this.existingPreference !== undefined)
    {
      let date = new Date(this.existingPreference.lastSent);  
    if(date.getTime() === new Date('1990-01-01').getTime())
        return "Not subscribed yet!";
      else
        return new Date(this.existingPreference.lastSent).toUTCString()
    }
    return "loading..."
  }

  savePreference()
  {
    let freq = this.newsletterForm.get('frequency')?.value;
    if(freq !== undefined && freq !== null && this.user !== null && this.user !== undefined)
    {
      this.existingPreference.userid = this.user.id;
      this.existingPreference.frequency = freq;
      this.existingPreference.lastSent = new Date()
      this.touristService.updateNewsletterPrefence(this.existingPreference).subscribe(
        (res: NewsletterPreference) => {
          alert("Successfully updated!");
        }
      );
    }
  }
}
