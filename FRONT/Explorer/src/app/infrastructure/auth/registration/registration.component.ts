import { Component, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Registration } from '../model/registration.model';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { MarketplaceService } from 'src/app/feature-modules/marketplace/marketplace.service';
import { Wallet } from 'src/app/feature-modules/marketplace/model/wallet.model';
import { MatDialog } from '@angular/material/dialog';
import { CartWarningComponent } from 'src/app/feature-modules/marketplace/dialogs/cart-warning/cart-warning.component';
import { RegisterMessageComponent } from 'src/app/feature-modules/marketplace/dialogs/register/register-message.component';

@Component({
  selector: 'xp-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})

export class RegistrationComponent {
  @Output() walletUpdated = new EventEmitter<null>(); 
  private lastWalletId: number;
  constructor(
    private authService: AuthService,
    private marketplaceService: MarketplaceService,
    private router: Router,
    private dialog: MatDialog,
  ) { this.lastWalletId = 0; }

  registrationForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    surname: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required]),
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required]),
    profileImage: new FormControl('', [Validators.required]),
    biography: new FormControl('', [Validators.required]),
    quote: new FormControl('', [Validators.required]),
  });

  register(): void {
    const registration: Registration = {
      name: this.registrationForm.value.name || "",
      surname: this.registrationForm.value.surname || "",
      email: this.registrationForm.value.email || "",
      username: this.registrationForm.value.username || "",
      password: this.registrationForm.value.password || "",
      profileImage: this.registrationForm.value.profileImage || "",
      biography: this.registrationForm.value.biography || "",
      quote: this.registrationForm.value.quote || ""
    };

    if (this.registrationForm.valid) {
      this.authService.register(registration).subscribe({
        next: (user) => {
          this.marketplaceService.getAllWallets().subscribe((wallets) => {
            if (wallets.results && wallets.results.length > 0) {
              const lastOrder = wallets.results[wallets.results.length - 1];
              this.lastWalletId = lastOrder.id + 1;
            } else {
              this.lastWalletId = 1;
            }
            const wallet: Wallet = {
              id: this.lastWalletId,
              userId: user.id, 
              adventureCoins: 0
            };
            this.marketplaceService.createWallet(wallet).subscribe({
              next: () => {
                this.walletUpdated.emit(); 
                //this.router.navigate(['home']);
                this.dialog.open(RegisterMessageComponent);
                this.router.navigate(['login']);
              },
            });
          });
        },
      });
    }
  }
}
