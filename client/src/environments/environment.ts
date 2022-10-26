// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api/',
  stripePublishableKey: 'pk_test_51Lne02H2yNW0vyBv2Dg8AgpUqkwahNvj1DMh0aj7Ecs1tI61s5nbMc78XqQtqcxQKVAGiFCMFjEhtf3sOC0xOEfV00x8ET5PYu',
  stripeSecretKey: 'sk_test_51Lne02H2yNW0vyBvT07WOElGYzWsndc1biW3Ax2kbIaNzkHmezMpJYPZHwutEFRn9WXdyd4IHyslOgCLxJnecKvZ00qNeOQRn5',
  stripeWebhookSecret: 'whsec_edeb5058adfa7ca6c484b000c139b76377a7a6b5a4166edf7951f808b90d8101'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
