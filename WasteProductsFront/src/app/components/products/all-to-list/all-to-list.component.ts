import { Component, ViewChild, OnInit } from '@angular/core';
import { Product } from '../../../models/products/product';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { ProductService } from '../../../services/product/product.service';
import { Router } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';


@Component({
  selector: 'app-all-to-list',
  templateUrl: './all-to-list.component.html',
  styleUrls: ['./all-to-list.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0', visibility: 'hidden' })),
      state('expanded', style({ height: '*', visibility: 'visible' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
  
  providers: [ProductService]
})
export class AllToListComponent implements OnInit {

  constructor (public productService: ProductService,
    private router: Router) {}

  products: Product[] = [];
  //userProducts: UserProduct[] = [];

  //data: UserProduct[] = this.userProducts;
  data: Product[] = this.products;
  dataSource = new MatTableDataSource(this.data);
  displayedColumns: string[] = ['Id', 'Name', 'AvgRating'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ngOnInit() {
    this.paginator.length = this.data.length;
    this.dataSource.sort = this.sort;

this.productService.getProducts().subscribe(
    res => {
      // this.userProducts = res;
      this.products = res;
    },
    err => console.error(err)
  );

// this.productService.getUserProducts().subscribe(
//     res => {
//       // this.userProducts = res;
//       this.products = res;
//       // tslint:disable-next-line:prefer-const
//       for (let item of res) {
//         this.products.push(item.Product);
//       }
//     } ,
//     err => console.error(err));
//   }

//     applyFilter(filterValue: any) {
//       this.dataSource.filter = filterValue.trim().toLowerCase();
//     }
    
//     showAllToTable() {
//       this.router.navigate(['/products/all-to-list']);
    }

}