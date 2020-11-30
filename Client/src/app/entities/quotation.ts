import { QuotationDetail } from './quotation-detail';

export interface Quotation {
    quotationId: number;
    sessionValue: string;
    generateDate: Date;
    client: string;
    ruc: string;
    seller: string;
    detail: [QuotationDetail[]];
}
