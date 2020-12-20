// Copyright 2017 Google Inc. All Rights Reserved.

#import "GADUMediationTestClient.h"
#import "UnityAppController.h"

@interface GADUMediationTestClient () <GMTSMediationTestSuiteDelegate> {
  NSString *_appID;
}

@end

@implementation GADUMediationTestClient

- (instancetype)initWithAppId:(NSString *)appId
              mediationClient:(GADUMTypeMediationTestClientRef *)mediationClient {
  self = [super init];
  if (self) {
    _mediationClient = mediationClient;
    _appID = appId;
  }
  return self;
}

- (void)showMediationTestSuite {
  UIViewController *rootController =
      ((UnityAppController *)[UIApplication sharedApplication].delegate).rootViewController;

  [GoogleMobileAdsMediationTestSuite presentWithAppID:_appID
                                     onViewController:rootController
                                             delegate:self];
}

- (void)mediationTestSuiteWasDismissed {
  if (self.screenDismissedCallback) {
    self.screenDismissedCallback(self.mediationClient);
  }
}

@end
