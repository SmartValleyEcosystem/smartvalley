import {
  Component, ElementRef, EventEmitter, forwardRef, Input, OnChanges, OnInit, Output,
  ViewChild
} from '@angular/core';
import {
  ControlValueAccessor, FormControl, FormGroup, NG_VALIDATORS, NG_VALUE_ACCESSOR,
} from '@angular/forms';

@Component({
  selector: 'app-member-upload-photo',
  templateUrl: './member-upload-photo.component.html',
  styleUrls: ['./member-upload-photo.component.scss'],
  providers: [
    {provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => MemberUploadPhotoComponent), multi: true},
    {provide: NG_VALIDATORS, useExisting: forwardRef(() => MemberUploadPhotoComponent), multi: true}
  ]
})
export class MemberUploadPhotoComponent implements ControlValueAccessor, OnChanges, OnInit {

  private _value: File;
  public imgUrl: string;

  @Input() accept: string;
  @Input() name: string;
  @Input() maxFileSize: number;
  @Input() form?: FormGroup;
  @Input() elementId: string;

  @ViewChild('input') inputElement: ElementRef;

  @Output() uploadHandler: EventEmitter<File> = new EventEmitter<File>();
  @Output() onRemove: EventEmitter<any> = new EventEmitter<any>();

  constructor(private nativeElement: ElementRef) {
  }

  ngOnInit() {
  }

  onChange(event) {
    this.uploadFile(event);
  }

  private uploadFile(event: any) {
    const reader = new FileReader();
    reader.onload = (e: Event & { target: { result: string } }) => {
      this.imgUrl = e.target.result;
      this.inputElement.nativeElement.value = null;
    };

    if ( event.srcElement ) {
      this.value = event.srcElement.files[0];
    } else {
      this.value = event.target.files[0];
    }

    if (this.maxFileSize < this.value.size) {
      this.deleteFile();
    }

    reader.readAsDataURL(this.value);
    this.uploadHandler.emit(this.value);
  }

  private deleteFile() {
    this.imgUrl = null;
    this.value = null;
    this.onRemove.emit();
  }

  public propagateChange: any = () => {}

  public validateFn: any = () => {}

  get value(): File | null {
    return this._value;
  }

  set value(value: File | null) {
    this._value = value;
    this.propagateChange(value);
  }

  public ngOnChanges(inputs) {
    this.propagateChange(this.value);
  }

  public writeValue(value: File | null | string) {
    if (value) {
      if (typeof value === 'string') {
        this.imgUrl = value;
        return;
      }
      this.value = value;
    }
  }

  public registerOnTouched() {
  }

  public validate(c: FormControl) {
    return this.validateFn(c);
  }

  registerOnChange(fn) {
    this.propagateChange = fn;
  }
}
