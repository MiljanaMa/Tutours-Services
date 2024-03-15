import { Component, OnInit } from '@angular/core';
import { Wallet } from '../model/wallet.model';
import { MarketplaceService } from '../marketplace.service';
import { AuthService } from 'src/app/infrastructure/auth/auth.service';

@Component({
  selector: 'app-wallet',
  templateUrl: './wallet.component.html',
  styleUrls: ['./wallet.component.css']
})
export class WalletComponent implements OnInit {
  // Assuming you have a user object in your component
  wallet: Wallet;
  loggedId: number; 
  constructor(private marketplaceService: MarketplaceService, private authService: AuthService) { }

  ngOnInit(): void {
    this.getWallet(); 
    this.loggedId = this.authService.user$.value.id; 
  }

  getWallet(): void {
    this.marketplaceService.getWalletForUser().subscribe({
      next:(result:Wallet)=>{
        this.wallet=result;
      },
      error:(err:any)=>{
        console.log(err);
      }
  });
  }
}
