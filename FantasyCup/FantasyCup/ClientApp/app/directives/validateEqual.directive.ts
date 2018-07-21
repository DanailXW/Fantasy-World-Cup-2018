import { Directive, forwardRef, Attribute } from '@angular/core';
import { Validator, AbstractControl, NG_VALIDATORS } from '@angular/forms';

@Directive({
    selector: '[validateEqual][formControlName], [validateEqual][formControl], [validateEqual][ngModel]',
    providers: [{ provide: NG_VALIDATORS, useExisting: forwardRef(() => EqualValidator), multi: true }]
})
export class EqualValidator implements Validator {

    constructor(
        @Attribute('validateEqual') public validateEqual: string,
        @Attribute('reverse') public reverse: string
    ) { }

    private get isReverse() {
        if (!this.reverse) return false;
        return this.reverse === 'true' ? true : false;
    }

    validate(c: AbstractControl): { [key: string]: any } | null {
        let val = c.value;

        let e = c.root.get(this.validateEqual);
        if (e && val !== e.value && !this.isReverse) return {
            validateEqual: false
        }

        if (e && val === e.value && this.isReverse) {

            if (e.errors) {
                delete e.errors['validateEqual'];
                if (!Object.keys(e.errors).length)
                    e.setErrors(null);
            }                
        }

        if (e && val === e.value && !this.isReverse) {
            if (c.errors && c.errors['validateEqual']) {
                delete c.errors['validateEqual'];
                if (!Object.keys(c.errors).length)
                    c.setErrors(null);
            }
        }

        if (e && val !== e.value && this.isReverse) {
            if (e.errors)
                e.errors['validateEqual'] = false;
            else
                e.setErrors({ validateEqual: false });
        }

        return null;

    }
}