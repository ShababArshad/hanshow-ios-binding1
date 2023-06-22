//
//  NFCManager.h
//  
//
//  Created by hanshow on 2023/5/18.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <HanshowNFCLib/HanshowNFCLib.h>

@interface NFCManager : NSObject
+ (instancetype)manager;
-(void)getEslIdWithFinishBlock:(void(^)(NSString *eslId))finishBlock;
@end


