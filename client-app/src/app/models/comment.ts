// semantic ui ile çakışmasın diye böyle yaptık
export interface ChatComment
{
    id: number;
    createdAt: Date;
    body: string;
    username: string;
    displayName: string;
    image: string;
}
