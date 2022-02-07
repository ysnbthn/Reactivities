// hata yapmamak için activity modelini yapıyoruz
// eğer yazım yanlışı yaparsak typescript tokadı basıyor
export interface Activity{
    id: string;
    title: string;
    date: string;
    description: string;
    category: string;
    city: string;
    venue: string;
}