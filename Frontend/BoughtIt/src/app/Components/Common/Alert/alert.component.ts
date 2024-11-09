import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MessageButtonType } from './Message';

@Component({
  selector: 'app-alert',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './alert.component.html',
  styleUrl: './alert.component.css'
})
export class AlertComponent {
  public title:string= ''
  public message:string=''
  public isVisible:boolean=false;
  public buttons: MessageButtonType[] = []
  showAlert(data:any){
    this.title=data.title;
    this.message=data.message;
    this.isVisible=true;
  }
  hideAlert(){
    this.title='';
    this.message='';
    this.isVisible=false;
  }
  onButtonClick(action: () => void) {
    action();
    // this.close.emit();
  }

  onClose() {
    // this.close.emit();
  }
}
