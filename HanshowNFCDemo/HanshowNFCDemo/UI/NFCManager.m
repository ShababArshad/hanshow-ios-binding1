//
//  NFCManager.m
//  
//
//  Created by hanshow on 2023/5/18.
//

#import "NFCManager.h"
#import <CoreNFC/CoreNFC.h>


#define WEAKSELF_DEFINE __weak __typeof(self)weakSelf = self;

typedef NS_ENUM(NSInteger,ManagerAction){
    kManagerActionGetEslId,
};

@interface NFCManager()<NFCTagReaderSessionDelegate>
@property(nonatomic,strong) NFCTagReaderSession *session;
@property(nonatomic) ManagerAction action;
@property(nonatomic,strong) id param;
@property(nonatomic,strong) id finishBlock;
@end
@implementation NFCManager


-(void)getEslIdWithFinishBlock:(void(^)(NSString *eslId))finishBlock{
    self.action = kManagerActionGetEslId;
    self.finishBlock = finishBlock;
    [self beginSession];
}

-(void)beginSession{
    
    self.session = [[NFCTagReaderSession alloc] initWithPollingOption:NFCPollingISO14443 delegate:self queue:dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0)];
    [self.session beginSession];
}

#pragma mark NFCTagReaderSessionDelegate


- (void)tagReaderSession:(NFCTagReaderSession *)session didDetectTags:(NSArray<__kindof id<NFCTag>> *)tags {
    NSLog(@"didDetectTags : %@",tags);
    id<NFCTag> firstTag = tags.firstObject;
    if(![NFCLib isSupportedTag:firstTag]){
        NSLog(@"This type of tag is not supported");
        return;
    }
    WEAKSELF_DEFINE
    if(_action == kManagerActionGetEslId){
        
        [[NFCLib sharedInstance]getEslIdAction:firstTag FinishBlock:^(NFCResponse *response) {
            NSLog(@"Get ESL ID result:%@",response);
            [weakSelf cleanSession];
        }];
    }
}


- (void)tagReaderSession:(NFCTagReaderSession *)session didInvalidateWithError:(NSError *)error {
    NSLog(@"The session was invalidated: %@", [error localizedDescription]);
    [self cleanSession];
}


- (void)tagReaderSessionDidBecomeActive:(NFCTagReaderSession *)session {
    NSLog(@"The session has become active");
}
-(void)cleanSession{
    [self.session invalidateSession];
    self.session = nil;
    NSLog(@"clean Session");
}

#pragma mark - singleInstanceMethod
static NFCManager *_singleInstance = nil;
+ (instancetype)manager{
    return [self sharedInstance];
}
+ (instancetype)sharedInstance {
    static NFCManager *sharedInstance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        sharedInstance = [[super allocWithZone:NULL] init];
    });
    return sharedInstance;
}

+ (id)allocWithZone:(struct _NSZone *)zone {
    return [self sharedInstance];
}

- (id)copyWithZone:(NSZone *)zone {
    return self;
}
@end
