package com.packageapp.dao;

import java.util.ArrayList;
import java.util.List;
import com.packageapp.model.Package;

public class PackageDaoImpl implements PackageDao {
    private List<Package> packageList = new ArrayList<>();

    @Override
    public void addPackage(Package pkg) {
        packageList.add(pkg);
    }

    @Override
    public List<Package> getAllPackages() {
        return packageList;
    }

    @Override
    public Package getPackageById(String id) {
        for (Package pkg : packageList) {
            if (pkg.getPackageId().equals(id)) {
                return pkg;
            }
        }
        return null;
    }

    @Override
    public void calculatePackageCost(Package pkg) {
        double basicCost = pkg.getBasicFare() * pkg.getNoOfDays();
        double discount = 0.0;
        int days = pkg.getNoOfDays();
        if (days > 5 && days <= 8) discount = basicCost * 0.03;
        else if (days > 8 && days <= 10) discount = basicCost * 0.05;
        else if (days > 10) discount = basicCost * 0.07;
        double discountedCost = basicCost - discount;
        double gst = discountedCost * 0.12;
        pkg.setPackageCost(discountedCost + gst);
    }
}