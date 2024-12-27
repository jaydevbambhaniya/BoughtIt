import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Order, OrderDto, OrderItemDto } from '../../Models/Order';
import { OrderService } from '../../Services/OrderService/order.service';
import { CommonModule } from '@angular/common';
import { User } from '../../Models/User';
import { UserService } from '../../Services/UserService/user.service';
import { LoadingSpinnerComponent } from "../Common/LoadingSpinner/loading-spinner.component";
import { AlertComponent } from "../Common/Alert/alert.component";
import { ErrorCodes } from '../../error-codes';


@Component({
  selector: 'app-orderdetails',
  standalone: true,
  imports: [CommonModule, LoadingSpinnerComponent, AlertComponent],
  templateUrl: './orderdetails.component.html',
  styleUrl: './orderdetails.component.css'
})
export class OrderdetailsComponent implements OnInit {
  @ViewChild('messageBox') messageBox!: AlertComponent;
  public userId: number=0;
  public orderId: number=0;
  public orders$:Observable<OrderDto[]> = of([]);
  public imageUrl='https://localhost:5000/static/';
  public isLoading:boolean=false;
  constructor(private route:ActivatedRoute,
    private orderService:OrderService,private userService:UserService,
  private router:Router)
  {}
  ngOnInit(): void {
    this.messageBox = new AlertComponent();
    this.route.queryParams.subscribe(params => {
      this.userId = params['userId'] || 0;
      this.orderId = params['orderId'] || 0;
      this.loadUserOrders();
    });
  }
  loadUserOrders(){
    this.orders$ = this.orderService.getUserOrders(this.userId,this.orderId);
  }
  getTotal(orderItems:OrderItemDto[]): number {
    if(orderItems.length==0)return 0;
    return orderItems.reduce((sum, oi) => sum + (oi.price * oi.quantity)-oi.discount, 0);
  }
  cancelOrder(orderId:number){
    this.isLoading=true;
    this.orderService.cancelOrder(orderId).subscribe({
      next:(response:number)=>{
        this.messageBox.buttons = [{text:'Okay', action:()=>this.onOrderCancel()}];
        var message='';
        if(response<0){
          message = ErrorCodes[`${response}`].message;
        }else{
          message = "Order Cancelled!!"
        }
        this.isLoading=false;
        this.messageBox.showAlert({title:'Cancel Order',message});
      },
      error:(error)=>{
        
        this.isLoading=false;
        this.messageBox.showAlert({title:'Cancel Order',message:'Error Ocurred!!'});
      }
    });
  }
  onOrderCancel(){
    window.location.reload();
    this.messageBox.hideAlert();
  }
}