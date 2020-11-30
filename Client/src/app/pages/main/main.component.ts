import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Product } from 'src/app/entities/product';

import { Quotation } from '../../entities/quotation';
import { ProductService } from '../../services/product.service';
import { QuotationService } from '../../services/quotation.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {

  showMessage: boolean = false;
  message: string = '';
  showError: boolean = false;
  messageTimer = null;
  Form: FormGroup = new FormGroup({
    ProductId: new FormControl(null, Validators.required),
    Quantity: new FormControl(0, Validators.compose([Validators.required, Validators.min(1), Validators.max(999999999)])),
    Amount: new FormControl(0, Validators.compose([Validators.required, Validators.min(1), Validators.max(999999999)])),
    Tax: new FormControl(0, Validators.compose([Validators.required, Validators.min(0), Validators.max(999999999)])),
    Discount: new FormControl(0, Validators.compose([Validators.required, Validators.min(0), Validators.max(999999999)]))
  });
  FormInfo: FormGroup = new FormGroup({
    Client: new FormControl('', Validators.compose([Validators.minLength(0), Validators.maxLength(50)])),
    Ruc: new FormControl('', Validators.compose([Validators.minLength(0), Validators.maxLength(20)])),
    Seller: new FormControl('', Validators.compose([Validators.minLength(0), Validators.maxLength(50)]))
  });
  Products: Product[] = [];
  loading: boolean = false;
  currentQuotation: Quotation;

  constructor(
    public productService: ProductService,
    public quotationService: QuotationService) { }

  ngOnInit() {
    this.InitAll();
  }

  InitAll() {
    this.FormInfo.reset();
    this.UpdateProducts();
    this.GetQuotation();
    this.CreateForm();
  }

  CreateForm() {
    this.Form.reset();
    this.Form.controls.ProductId.setValue(null);
    this.Form.controls.Quantity.setValue(0);
    this.Form.controls.Amount.setValue(0);
    this.Form.controls.Tax.setValue(0);
    this.Form.controls.Discount.setValue(0);
  }

  GetQuotation() {
    this.loading = true;
    this.quotationService.Get()
      .subscribe(
        resp => {
          if (resp.ok) {
            this.currentQuotation = resp.body;
            this.FormInfo.controls.Client.setValue(this.currentQuotation.client);
            this.FormInfo.controls.Ruc.setValue(this.currentQuotation.ruc);
            this.FormInfo.controls.Seller.setValue(this.currentQuotation.seller);
          }
          this.loading = false;
        },
        (err) => {
          this.Alert('Error al obtener la informacion', true);
        }
      );
  }

  Print() {
    window.print();
  }

  UpdateProducts() {
    this.loading = true;
    this.productService.Get()
      .subscribe(
        resp => {
          if (resp.ok) {
            this.Products = resp.body;
          }
          this.loading = false;
        },
        (err) => {
          this.Alert('Error al consultar los productos', true);
        }
      );
  }

  ChangeItem() {
    if (this.FormInfo.valid) {
      this.currentQuotation.client = this.FormInfo.controls.Client.value;
      this.currentQuotation.ruc = this.FormInfo.controls.Ruc.value;
      this.currentQuotation.seller = this.FormInfo.controls.Seller.value;
      this.quotationService.Post(this.currentQuotation)
        .subscribe(
          resp => {
            if (resp.ok) {
              this.CreateForm();
              this.Alert('Registro modificado exitosamente');
            } else {
              this.Alert('Registro con error al modificar', true);
            }
            this.GetQuotation();
          },
          (err) => {
            this.Alert('Registro con error al modificar', true);
          }
        );
    }
  }

  DeleteAll() {
    this.loading = true;
    this.quotationService.Delete()
      .subscribe(
        resp => {
          if (resp.ok) {
            this.InitAll();
            this.Alert('Cotizacion eliminada');
          } else {
            this.Alert('Cotizacion con error al eliminar', true);
          }
          this.GetQuotation();
        },
        (err) => {
          this.Alert('Cotizacion con error al eliminar', true);
        }
      );
  }

  Save() {
    if (this.Form.valid) {
      this.loading = true;
      this.currentQuotation.detail.push(this.Form.value);
      this.quotationService.Post(this.currentQuotation)
        .subscribe(
          resp => {
            if (resp.ok) {
              this.CreateForm();
              this.Alert('Registro guardado exitosamente');
            } else {
              this.Alert('Registro con error al guardar', true);
            }
            this.GetQuotation();
          },
          (err) => {
            this.Alert('Registro con error al guardar', true);
          }
        );
    } else {
      this.Alert('Valide todos los datos.', true);
    }
  }

  changeProduct() {
    const value = this.Form.controls.ProductId.value;
    const item = this.Products.find((Obj) => Obj.productId === value);
    if (item) {
      this.Form.controls.Amount.setValue(item.price);
    }
  }

  removeProduct(idx) {
    this.currentQuotation.detail.splice(idx, 1);
    this.Alert('Registro eliminado exitosamente');
  }

  GetErrorsClass(formControlName) {
    return this.Errors(formControlName, true);
  }

  GetErrors(formControlName) {
    return this.Errors(formControlName);
  }

  private Errors(formControlName, isClass = false) {
    try {
      // tslint:disable-next-line: no-string-literal tslint:disable-next-line: deprecation  tslint:disable-next-line: max-line-length
      if ((event.target['closest']('form') || event.target['closest']('.modal').querySelector('form'))['getAttribute']('id')) {
        // tslint:disable-next-line: max-line-length
        // tslint:disable-next-line: no-string-literal  tslint:disable-next-line: deprecation  tslint:disable-next-line: max-line-length
        const el = this[((event.target['closest']('form') || event.target['closest']('.modal').querySelector('form'))['getAttribute']('id'))].controls[formControlName];
        if (el) {
          if (el.invalid && (el.dirty || el.touched)) {
            if (el.errors) {
              if (isClass) {
                return 'error-input';
              }
              if (el.errors.required) {
                return '<label>El campo es requerido</label>';
              } else if (el.errors.minlength) {
                return '<label>la longitud minima es ' + el.errors.minlength.requiredLength + '</label>';
              } else if (el.errors.maxlength) {
                return '<label>la longitud maxima es' + el.errors.maxlength.requiredLength + '</label>';
              } else if (el.errors.min) {
                return '<label>el valor minimo es ' + el.errors.min.min + '</label>';
              } else if (el.errors.max) {
                return '<label>el valor maximo es ' + el.errors.max.max + '</label>';
              }
              return '<label>campo invalido</label>';
            }
          }
        } else {
          console.warn('Error', 'el control "' + formControlName + '" no se ecnuentra');
        }
      } else {
        console.warn('Error', 'No se encuentra el id');
      }
      return '';
    } catch {
      return '';
    }
  }

  Alert(msg: string, error: boolean = false) {
    this.showError = error;
    this.message = msg;
    this.showMessage = true;
    this.loading = false;
    this.messageTimer = setTimeout(() => {
      this.showMessage = false;
    }, 5000);
  }
}
