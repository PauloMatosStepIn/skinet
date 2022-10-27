import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/products';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm: ElementRef;
  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  // shopParams = new ShopParams();
  shopParams: ShopParams;
  totalCount: number;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: Low to High', value: 'priceAsc' },
    { name: 'Price: High to Low', value: 'priceDesc' }
  ];
  constructor(private shopService: ShopService) {

    this.shopParams = this.shopService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts(true);
    this.getBrands();
    this.getTypes();
  }

  getProducts(useCache = false) {
    this.shopService.getProducts(useCache
    )
      .subscribe(response => {
        this.products = response.data;
        // this.shopParams.pageNumber = response.pageIndex;
        // this.shopParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      }, error => {
        console.log(error);
      }
      )
  }

  getBrands() {
    this.shopService.getBrands().subscribe(response => {
      //Add ALL element to the response brands array
      this.brands = [{ id: 0, name: 'All' }, ...response]
    }, error => {
      console.log(error);
    })
  }

  getTypes() {
    this.shopService.getTypes().subscribe(response => {
      //Add ALL element to the response types array
      this.types = [{ id: 0, name: 'All' }, ...response]
    }, error => {
      console.log(error);
    })
  }
  onBrandSelected(brandId: number) {
    // this.shopParams.brandId = brandId;
    // this.shopParams.pageNumber = 1;
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);

    this.getProducts();
  }


  onTypeSelected(typeId: number) {
    // this.shopParams.typeId = typeId;
    // this.shopParams.pageNumber = 1;
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);

    this.getProducts();
  }

  onSortSelected(sort: string) {
    // this.shopParams.sort = sort;
    const params = this.shopService.getShopParams();
    params.sort = sort;
    this.shopService.setShopParams(params);

    this.getProducts();
  }

  onPageChanged(event: any) {
    //event from pagination component

    if (this.shopParams.pageNumber !== event) {
      // this.shopParams.pageNumber = event;
      const params = this.shopService.getShopParams();
      params.pageNumber = event;
      this.shopService.setShopParams(params);

      this.getProducts(true);
    }

  }

  onSearch() {
    // this.shopParams.search = this.searchTerm.nativeElement.value;
    // this.shopParams.pageNumber = 1;

    const params = this.shopService.getShopParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);

    this.getProducts();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';

    // const params = new ShopParams();
    // this.shopService.setShopParams(params);
    this.shopParams = new ShopParams();
    this.shopService.setShopParams(this.shopParams);

    this.getProducts();
  }
}
