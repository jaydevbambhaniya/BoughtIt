import { Product } from "./Product";

export class OrderItem{
    globalProductId: string = '';
    price: number=0;
    discount: number=0;
    quantity: number=0;
}
export class Order{
    userID: number = 0;
    orderItems: OrderItem[]=[];
    addressLine1:string='';
    addressLine2:string='';
    district:string='';
    city:string='';
    state:string='';
    postalCode:string='';
    country:string='India';
    orderID:number=0;
}

export class OrderItemDto{
    product: Product=new Product();
    price: number=0;
    discount: number=0;
    quantity: number=0;
}
export class OrderDto{
    userID: number = 0;
    orderItems: OrderItemDto[]=[];
    addressLine1:string='';
    addressLine2:string='';
    orderDate: string='';
    orderStatus: string='';
    deliveryDate: string='';
    district:string='';
    city:string='';
    state:string='';
    postalCode:string='';
    country:string='India';
    orderID:number=0;
}