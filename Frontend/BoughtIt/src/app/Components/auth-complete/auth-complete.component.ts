import { Component, OnInit } from '@angular/core';
import { LoadingSpinnerComponent } from '../Common/LoadingSpinner/loading-spinner.component';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../Services/UserService/user.service';
import { response } from 'express';

@Component({
  selector: 'app-auth-complete',
  standalone: true,
  imports: [LoadingSpinnerComponent],
  templateUrl: './auth-complete.component.html',
  styleUrl: './auth-complete.component.css'
})
export class AuthCompleteComponent implements OnInit {
  constructor(private route:ActivatedRoute,private userService:UserService,
    private router:Router
  ){}
  ngOnInit(): void {
    this.route.queryParams.subscribe((params)=>{
      const code = params['code'];
      console.log(code);
      if(code){
        this.userService.externalLogin(code).subscribe((reponse)=>{
          console.log(response);
          if(reponse>0)
            this.router.navigateByUrl('/home');
          else this.router.navigateByUrl('/login');
        });
      }
    })
  }
}
