import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  visible:boolean=true;
  constructor(private router:Router){}

  searchProducts(query:string){
    this.router.navigateByUrl('/products?searchText='+query);
  }
  toggleVisible(){
    this.visible = !this.visible;
  }
}
