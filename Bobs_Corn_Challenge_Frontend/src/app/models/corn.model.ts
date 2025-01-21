export interface CornPurchase {
    id: number;
    clientId: string;
    purchaseTime: Date;
    isSuccessful: boolean;
}

export interface PurchaseResponse {
    isSuccessful: boolean;
    message: string;
    timestamp: Date;
}

export interface ClientStats {
    totalPurchases: number;
    successfulPurchases: number;
    lastPurchaseTime: Date;
}