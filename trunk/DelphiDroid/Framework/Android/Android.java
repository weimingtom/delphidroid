package com.test;

import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.content.DialogInterface;
import android.app.Activity;

public class Android
{
  private android.app.Activity owner = null;
 
  public Android(android.app.Activity aOwner) 
  {
	  owner = aOwner;
  }

  public void AlertBox(String aText)
  {
		AlertDialog.Builder alt_bld = new AlertDialog.Builder(owner);
		alt_bld.setMessage(aText);
		alt_bld.setCancelable(false);
		alt_bld.setPositiveButton("Ok", new DialogInterface.OnClickListener() {
		public void onClick(DialogInterface dialog, int id) {
			// Action for 'Yes' Button
		}
		});
		alt_bld.setNegativeButton("Cencel", new DialogInterface.OnClickListener() {
		public void onClick(DialogInterface dialog, int id) {
			//  Action for 'NO' Button
		dialog.cancel();
		}
		});

		AlertDialog alert = alt_bld.create();
		// Title for AlertDialog
		alert.setTitle("Title");
		// Icon for AlertDialog
		alert.show();
  }
}