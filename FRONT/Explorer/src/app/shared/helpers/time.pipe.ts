import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'time',
  standalone: true
})
export class TimePipe implements PipeTransform {

  transform(value: number): string {
    if(value >= 60){
      return `${Math.floor(value/60)}h ${value%60}min`;
    }
     
    return `${value}min`;
  }

}
