import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Product } from '../../Models/Product';
import { OrderService } from '../../Services/OrderService/order.service';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CartService } from '../../Services/CartService/cart.service';
import { Order, OrderItem } from '../../Models/Order';
import { BrowserStorageService } from '../../Services/BrowserStorage/browserstorage.service';
import { User } from '../../Models/User';
import { AlertComponent } from '../Common/Alert/alert.component';
import { LoadingSpinnerComponent } from "../Common/LoadingSpinner/loading-spinner.component";

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule, AlertComponent, LoadingSpinnerComponent],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent implements OnInit{
  @ViewChild('messageBox') messageBox!: AlertComponent;
  public isLoading:boolean=false;
  public products : Product[] = [];
  public fileBaseUrl = 'https://localhost:5000/static/';
  public orderForm:FormGroup;
  constructor(private orderService:OrderService,private cartService:CartService,
    private browserStorage:BrowserStorageService, private router:Router
  ){
    this.orderForm = new FormGroup({
      firstname : new FormControl('',[Validators.required]),
      lastname: new FormControl('',[Validators.required]),
      email: new FormControl('',[Validators.required,Validators.email]),
      phone: new FormControl('',[Validators.required]),
      addressLine1: new FormControl('',[Validators.required]),
      addressLine2: new FormControl('',[Validators.required]),
      state: new FormControl('',[Validators.required]),
      city: new FormControl('',[Validators.required]),
      zipcode: new FormControl('',[Validators.required]),
      district: new FormControl('',[Validators.required]),
      agreement: new FormControl('',[Validators.requiredTrue])
    });
  }
  ngOnInit(): void {
    this.messageBox = new AlertComponent();
    this.products = this.cartService.getProductsToBuy();
    
  }

  onPlaceOrderClick(){
    this.messageBox.buttons = [{ text: 'Place Order', primary: true, action: () => this.placeOrderConfirm() },
      {text:'Cancel',primary:false,action:()=>this.messageBox.hideAlert()}
    ];
    this.messageBox.showAlert({title:'Order Confirmation',message:'Please confirm you order details.'});
  }
  placeOrderConfirm(){
    this.messageBox.hideAlert();
    this.isLoading=true;
    var user = JSON.parse(this.browserStorage.get('currentUser')||'') as User;
    var order = new Order();
    order.userID = user.id;
    var orderItems:OrderItem[]=[];
    this.products.forEach(p=>{
      var orderItem = new OrderItem();
      orderItem.globalProductId = p.globalProductId;
      orderItem.quantity = p.quantity;
      orderItem.price = p.price;
      orderItem.discount = 0;
      orderItems.push(orderItem);
    });
    order.orderItems = orderItems;
    order.addressLine1 = this.orderForm.get('addressLine1')?.value;
    order.addressLine2 = this.orderForm.get('addressLine2')?.value;
    order.city = this.orderForm.get('city')?.value;
    order.district = this.orderForm.get('district')?.value;
    order.state = this.orderForm.get('state')?.value;
    order.postalCode = this.orderForm.get('zipcode')?.value;

    this.orderService.placeOrder(order).subscribe({
      next:(response)=>{
        this.isLoading=false;
        if(response.orderID>0){
          this.messageBox.buttons = [{ text: 'Okay', primary: true, action: () => this.redirectToOrderDetails(response.orderID) }];
          this.messageBox.showAlert({title:'Order', message:'Order Placed successfully.'});
        }else{
          this.messageBox.buttons = [{ text: 'Okay', primary: true, action: () => this.messageBox.hideAlert() }];
          this.messageBox.showAlert({title:'Order', message:'Error occurred! please try again after some time.'});
        }
      },
      error:(error)=>{
        console.log(error);
        this.isLoading=false;
      }
    });
  }
  redirectToOrderDetails(orderId: number){
    this.messageBox.hideAlert();
    this.router.navigateByUrl('/orderDetails?orderId='+orderId);
  }
  getTotal(): number {
    return this.products.reduce((sum, product) => sum + (product.price * product.quantity), 0);
  }
}
