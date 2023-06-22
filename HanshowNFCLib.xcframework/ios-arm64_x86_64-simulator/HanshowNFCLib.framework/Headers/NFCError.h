//
//  NFCError.h
//  
//
//  Created by hanshow on 2023/5/19.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger,NFCErrorType){
    kNFCUnavailable,
    kNFCNotSupport,
    kNFCDisconnect,
    kNFCTimeout,
    kNFCNotConfigured,
    kNFCImageError,
};

@interface NFCError : NSObject
@property(nonatomic) NFCErrorType errorType;
@property(nonatomic,copy)NSString *desc;
+(instancetype)errorWithType:(NFCErrorType)type Desc:(NSString*)desc;
@end


