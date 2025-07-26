package com.packageapp;

import java.util.Scanner;
import java.util.List;
import com.packageapp.model.Package;
import com.packageapp.service.PackageService;
import com.packageapp.service.PackageServiceImpl;
import com.packageapp.exception.InvalidPackageIdException;

public class Main {
    public static void main(String[] args) {
        Scanner sc = new Scanner(System.in);
        PackageService service = new PackageServiceImpl();

        while (true) {
            System.out.println("\n----- Holiday Package System -----");
            System.out.println("1. Add Package Details");
            System.out.println("2. Display All Packages");
            System.out.println("3. Search Package by ID");
            System.out.println("4. Calculate Package Cost by ID");
            System.out.println("5. Exit");
            System.out.print("Enter your choice: ");
            int choice = sc.nextInt();

            try {
                switch (choice) {
                    case 1:
                        sc.nextLine();
                        System.out.print("Enter Package ID: ");
                        String id = sc.nextLine();
                        System.out.print("Enter Source Place: ");
                        String source = sc.nextLine();
                        System.out.print("Enter Destination Place: ");
                        String dest = sc.nextLine();
                        System.out.print("Enter No of Days: ");
                        int days = sc.nextInt();
                        System.out.print("Enter Basic Fare: ");
                        double fare = sc.nextDouble();
                        Package pkg = new Package(id, source, dest, days, fare);
                        service.addPackage(pkg);
                        System.out.println("Package Added Successfully!");
                        break;

                    case 2:
                        List<Package> allPackages = service.fetchAllPackages();
                        for (Package p : allPackages) {
                            System.out.println(p);
                        }
                        break;

                    case 3:
                        sc.nextLine();
                        System.out.print("Enter Package ID to Search: ");
                        String searchId = sc.nextLine();
                        Package found = service.findPackageById(searchId);
                        if (found != null) System.out.println(found);
                        else System.out.println("Package Not Found.");
                        break;

                    case 4:
                        sc.nextLine();
                        System.out.print("Enter Package ID to Calculate Cost: ");
                        String calcId = sc.nextLine();
                        service.calculateCost(calcId);
                        Package updated = service.findPackageById(calcId);
                        if (updated != null) {
                            System.out.println("Updated Package Cost: " + updated.getPackageCost());
                        }
                        break;

                    case 5:
                        System.out.println("Thank You for using our App");
                        sc.close();
                        System.exit(0);

                    default:
                        System.out.println("Invalid choice!");
                }
            } catch (InvalidPackageIdException e) {
                System.out.println("Error: " + e.getMessage());
            }
        }
    }
}