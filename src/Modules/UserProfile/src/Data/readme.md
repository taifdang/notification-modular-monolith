add-migration initial -Context UserProfileDbContext -Project UserProfile -StartupProject Api -o Data\Migrations
update-database -Context UserProfileDbContext


{
	"channels":{
		"email": true,
		"sms": false,
		"push": true
	},
	"events":{
		"promotions":false,
		"topup_created":true,
	}
}