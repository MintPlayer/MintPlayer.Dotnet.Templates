export interface User {
  id: string;
  email: string | null;
  firstName: string | null;
  lastName: string | null;
  emailConfirmed: boolean;
  twoFactorEnabled: boolean;
//#if (EnableConcurrencyHandling)
  concurrencyStamp: string | null;
//#endif
}
